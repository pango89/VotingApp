using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Worker.Data;

namespace worker
{
    public class QueueWorker
    {
        private readonly IConfiguration _config;
        private readonly IVoteData _data;
        private readonly ILogger _logger;
        private readonly IRedisStore _redisStore;

        public QueueWorker(IConfiguration config, IVoteData data, ILogger<QueueWorker> logger, IRedisStore redisStore)
        {
            _config = config;
            _data = data;
            _logger = logger;
            _redisStore = redisStore;
        }

        public void Start()
        {
            _logger.LogInformation($"Connecting to redis queue url: {_config.GetValue<string>("Redis:Url")}");
            
            // Create subscriber
            var pubsub = _redisStore.GetConnection().GetSubscriber();

            // Subscriber subscribes to a channel
            var redisChannel = _config.GetValue<string>("Redis:Channel");
            pubsub.Subscribe(redisChannel, (channel, message) => SaveVote(message));
        }

        private void SaveVote(string json)
        {
            var definition = new { vote = "", voter_id = "" };
            var voteMessage = JsonConvert.DeserializeAnonymousType(json, definition);
            _logger.LogInformation($"Processing vote for '{voteMessage.vote}' by '{voteMessage.voter_id}'");
            try
            {
                _data.Set(voteMessage.voter_id, voteMessage.vote);
                _logger.LogDebug($"Successfuly processed vote by '{voteMessage.voter_id}'");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Vote processing FAILED for '{voteMessage.voter_id}', exception: {ex}");
            }
        }
    }
}

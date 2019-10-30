using Amazon.DynamoDBv2.DataModel;

namespace IoT_Gas.Entities
{
    [DynamoDBTable("GasDB")]
    public class Sample
    {
        [DynamoDBHashKey]
        public string Key { get; set; }
        [DynamoDBRangeKey]
        public int counter { get; set; }

        [DynamoDBProperty]
        public string gas { get; set; }

        [DynamoDBProperty]
        public string time { get; set; }

        [DynamoDBProperty]
        public string lat { get; set; }

        [DynamoDBProperty]
        public string lng { get; set; }

        public override string ToString()
        {
            return string.Format(@"{0} – {1} Gas:  {2}, Time: {3}", Key, counter, gas, time);
        }

    }


}
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.DocumentModel;
using IoT_Gas.DynamoDb;
using IoT_Gas.Entities;

namespace IoT_Gas.CRUD
{
    public class CRUDSample
    {
        private readonly DynamoService _dynamoService;

        public CRUDSample() => _dynamoService = new DynamoService();

        /// <summary>
        ///  AddSample will accept a Sample object and creates an Item on Amazon DynamoDB
        /// </summary>
        /// <param name="sample"></param>
        public void AddSample(Sample sample)
        {
            _dynamoService.Store(sample);
        }

        /// <summary>
        /// ModifySample  tries to load an existing Sample, modifies and saves it back. If the Item doesn’t exist, it raises an exception
        /// </summary>
        /// <param name="sample"></param>
        public void ModifySample(Sample sample)
        {
            _dynamoService.UpdateItem(sample);
        }

        /// <summary>
        /// GetALllSamples will perform a Table Scan operation to return all the Samples
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Sample> GetAllSamples()
        {
            return _dynamoService.GetAll<Sample>();
        }

        public IEnumerable<Sample> SearchSamples(string row, int positionInRow)
        {
            IEnumerable<Sample> filteredSamples = _dynamoService.DbContext.Query<Sample>(row, QueryOperator.Equal, positionInRow);

            return filteredSamples;
        }

        public IEnumerable<Sample> GetSamplesOfDevice(string id)
        {
            IEnumerable<Sample> filteredSamples = _dynamoService.DbContext.Query<Sample>(id);

            return filteredSamples;
        }

        public class Device
        {
            public string id;
            public string lat;
            public string lng; 

            public Device(string id, string lat, string lng)
            {
                this.id = id;
                this.lat = lat;
                this.lng = lng;
            }
            public override bool Equals(object obj)
            {
                if(obj is Device)
                {
                    Device device = (Device) obj;
                    if (this.id == device.id && this.lat == device.lat && this.lng == device.lng)
                        return true;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return id.GetHashCode() + lng.GetHashCode() + lat.GetHashCode();
            }
        }
        public IEnumerable<Device> GetDevicesID()
        {
            IEnumerable<Device> filteredSamples = _dynamoService.GetAll<Sample>().Select(s => new Device ( s.Key, s.lat, s.lng )).Distinct();

            return filteredSamples;
        }

        /// <summary>
        /// Delete Sample will remove an item from DynamoDb
        /// </summary>
        /// <param name="sample"></param>
        public void DeleteSample(Sample sample)
        {
            _dynamoService.DeleteItem(sample);
        }

        #region TODO
        //public List<DVD> SearchDvdByTitle(string title)
        //{
        //    // Define item hash-key
        //    var hashKey = new AttributeValue { S = title };

        //    // Create the key conditions from hashKey
        //    var keyConditions = new Dictionary<string, Condition>
        //    {
        //        // Hash key condition. ComparisonOperator must be "EQ".
        //        { 
        //            "Title",
        //            new Condition
        //            {
        //                ComparisonOperator = "EQ",
        //                AttributeValueList = new List<AttributeValue> { hashKey }
        //            }
        //        }
        //    };

        //    // Define marker variable
        //    Dictionary<string, AttributeValue> startKey = null;

        //    do
        //    {
        //        // Create Query request
        //        var request = new QueryRequest
        //        {
        //            TableName = "DVD",
        //            ExclusiveStartKey = startKey,
        //            KeyConditions = keyConditions
        //        };

        //        // Issue request
        //        var result = _dynamoService.DynamoClient.Query(request);

        //        // View all returned items
        //        List<Dictionary<string, AttributeValue>> items = result.Items;
        //        foreach (Dictionary<string, AttributeValue> item in items)
        //        {
        //            foreach (var keyValuePair in item)
        //            {
        //                Console.WriteLine("{0} : S={1}, N={2}, SS=[{3}], NS=[{4}]",
        //                    keyValuePair.Key,
        //                    keyValuePair.Value.S,
        //                    keyValuePair.Value.N,
        //                    string.Join(", ", keyValuePair.Value.SS ?? new List<string>()),
        //                    string.Join(", ", keyValuePair.Value.NS ?? new List<string>()));
        //            }
        //        }

        //        // Set marker variable
        //        startKey = result.LastEvaluatedKey;
        //    } while (startKey != null && startKey.Count > 0);

        //    return new List<DVD>();

        //}
        #endregion

    }
}
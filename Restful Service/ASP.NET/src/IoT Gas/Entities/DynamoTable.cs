﻿namespace IoT_Gas.Entities
{
    public class DynamoTable
    {
        public string PrimaryKey { get; set; }
        public string HashKey { get; set; }
        public string RangeKey { get; set; }
    }
}
#!/usr/bin/env python
# coding: utf-8

# In[1]:


import boto3
import json


# In[2]:



# Get the service resource.
from boto3.dynamodb.conditions import Key, Attr
dynamodb = boto3.resource('dynamodb')


# In[3]:



# Instantiate a table resource object without actually
# creating a DynamoDB table. Note that the attributes of this table
# are lazy-loaded: a request is not made nor are the attribute
# values populated until the attributes
# on the table resource are accessed or its load() method is called.
table = dynamodb.Table('GasDB')


# In[ ]:


from flask import Flask, request
from flask_restful import Resource, Api
from flask_cors import CORS
from flask import jsonify, make_response
from flask_json import FlaskJSON, JsonError, json_response, as_json

app = Flask(__name__)
api = Api(app)
CORS(app) 


#gas sample class
class GS(object):
    def __init__(self, id,counter,value,time):
        self.id = id
        self.counter = counter
        self.value = value
        self.time = time
        
    def serialize(self):
        return {
            'id': self.id, 
            'counter': self.counter,
            'value': self.value,
            'time' : self.time
        }

class GasSamples(Resource):
    def get(self, key_id):
        response = table.query(
            KeyConditionExpression=Key('Key').eq(key_id)
        )
        items = response['Items']
        
        sample = []
        for savedSample in items:
            sample.append(GS(savedSample['Key'],str(savedSample['counter']),str(savedSample['gas']), savedSample['time'][0:10] +" "+ savedSample['time'][11:19]))
           
        return jsonify( data =[e.serialize() for e in sample]) 


#device class
class Dev(object):
    def __init__(self, id,lat,lng):
        self.id = id
        self.lat = lat
        self.lng = lng 
    
    def serialize(self):
        return {
            'id': self.id, 
            'lat': self.lat,
            'lng': self.lng,
        }
    
class Devices(Resource):
    def get(self):
        response = table.scan(
            AttributesToGet=[
                'Key','lat','lng',
            ],
            Select='SPECIFIC_ATTRIBUTES'
        )
        items = response['Items'] 
        
        devices = []
        
        seen = set() 
        for d in items:
            t = tuple(d.items())
            if t not in seen:
                seen.add(t)
                devices.append(Dev(d['Key'],str(d['lat']),str(d['lng'])))
                               
        return jsonify( data = [e.serialize() for e in devices]) 
        

api.add_resource(Devices, '/devices') # Route_1
api.add_resource(GasSamples, '/gassamples/<key_id>') # Route_2


if __name__ == '__main__':
     app.run(port='5002')


# In[ ]:





# In[ ]:





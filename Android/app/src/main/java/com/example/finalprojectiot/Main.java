package com.example.finalprojectiot;


import android.content.Intent;
import android.os.Bundle;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;

import com.example.finalprojectiot.service.AWSService;

public class Main extends AppCompatActivity {

    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.content_main);


        Intent serviceIntent = new Intent(this, AWSService.class);
        serviceIntent.putExtra("inputExtra", "Service for Gas Monitoring");

        ContextCompat.startForegroundService(this, serviceIntent);
    }
}

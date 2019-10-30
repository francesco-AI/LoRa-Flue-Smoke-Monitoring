<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IoT_Gas.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        /* Set the size of the div element that contains the map */
        #map {
            height: 400px; /* The height is 400 pixels */
            width: 100%; /* The width is the width of the web page */
        }
    </style>
    <!-- jquery -->
    <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.4.1.min.js"></script>
    
    <!-- bootstrap -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/v/dt/dt-1.10.18/datatables.min.css" />
    <script type="text/javascript" src="https://cdn.datatables.net/v/dt/dt-1.10.18/datatables.min.js"></script>
  
    
        <script> 
            var map;
            var table;

            var marker;
            // Initialize and add the map
            function initMap() {
                if (navigator.geolocation) {
                    navigator.geolocation.getCurrentPosition(function (position) {
                        var pos = {
                            lat: position.coords.latitude,
                            lng: position.coords.longitude
                        };
                        map = new google.maps.Map(
                            document.getElementById('map'), { zoom: 4, center: pos });
                    });
                }


                $.ajax({
                    type: "GET",
                    url: "http://127.0.0.1:5002/devices",
                    dataType: "json",
                    data: "data",
                    success: function (result) { 
                        for (var i = 0; i < result.data.length; i++) {
                            var latLng = new google.maps.LatLng(result.data[i].lat, result.data[i].lng);
                            marker = new google.maps.Marker({ position: latLng, map: map, title: result.data[i].id });
                            marker.addListener('click', toggleBounce);
                        }
                    },
                    error: function (error) {
                        alert(error);
                    },
                });

                table = $('#example').DataTable({
                    dataSrc: "data",
                    columns: [
                        { data: "id", title: "ID" },
                        { data: "counter", title: "Counter" },
                        { data: "value", title: "Gas Value" },
                        { data: "time", title: "Time" }
                    ]
                });

                //data(results);
            }
            function toggleBounce() { 
                $("#dvMap").removeClass("col-12");
                $("#dvMap").addClass("col-6");
                $("#dvTable").addClass("col-6");
                $("#dvTable").show();
                table.ajax.type = "GET";
                table.ajax.url('http://127.0.0.1:5002/gassamples/' + this.title).load();
                table.draw();

            }


        </script>
        
</head>
<body>
    <form id="form1" runat="server">
        <h3>My Google Maps Demo</h3>
        <!--The div element for the map -->
        <div class="row" style="height:400px">
            <div id="dvMap" class="col-12">
                <div id="map"></div>
            </div>
            <div id="dvTable" style="display:none" >
                <table id="example" class="display"></table>
            </div>
        </div>
        
        <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBWIIZVkdGedNYWMLxRiHQHIavvIxEYKyo&libraries=visualization&callback=initMap"></script> 
     
      
    </form>
</body>
</html>

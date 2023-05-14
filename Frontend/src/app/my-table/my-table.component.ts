import { Component, OnInit } from '@angular/core';
import { LocalDataSource } from 'angular2-smart-table';
import Flight from '../model/flight.model';
import { DatePipe } from '@angular/common';
import { SignalrService } from '../service/signalr.service';
import { HttpClient } from '@angular/common/http';

export let _settings = {
  actions:{
    add: false,
    edit: false,
    delete: false,
  },
  columns: {
    flightNumber: {
      title: 'Flight Number'
    },
    leg: {
      title: 'Leg'
    },
    isArrival: {
      title: 'Is Arrival'
    },
    airLine: {
      title: 'Airline'
    },
    createdAt:  {
      title: 'Created At',
      valuePrepareFunction: (createdAt: Date) => {
        var raw = new Date(createdAt);
        console.log(raw);
        var formatted = new DatePipe('en-EN').transform(raw, 'dd MMM yyyy HH:mm:ss');
        return formatted;
    }
    }
  },
};

export let _data = [
];

@Component({
  selector: 'app-my-table',
  templateUrl: './my-table.component.html',
  styleUrls: ['./my-table.component.css'],
  providers: [DatePipe]
})

export class MyTableComponent implements OnInit{
  settings = _settings;

  source: LocalDataSource;
  data:any;
  flights : Flight[] = []
  constructor(private datePipe: DatePipe, public signalRService: SignalrService, private http: HttpClient) {
    this.source = new LocalDataSource(_data); 
  }
ngOnInit() {
  this.signalRService.startConnection();
  this.signalRService.addTransferFlightDataListener();  
  this.signalRService.updateTable.subscribe((data: Flight[]) => {
    this.data = data;
    this.source.load(this.data)
  });
}
}

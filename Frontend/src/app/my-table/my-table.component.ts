import { Component, OnInit } from '@angular/core';
import { LocalDataSource } from 'angular2-smart-table';
import { FlightService } from '../service/flight.service';
import Flight from '../model/flight.model';
import { NgFor } from '@angular/common';
import { DatePipe } from '@angular/common';

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
  constructor(private flightSrevice: FlightService,private datePipe: DatePipe) {
    this.source = new LocalDataSource(_data); 
  }
  ngOnInit(): void {
    this.flightSrevice.get().subscribe(data =>{
      console.log(data);
      this.data = data;
      this.source.load(this.data)
  });

  
}
}

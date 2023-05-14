import { EventEmitter, Injectable, Output } from '@angular/core';
import * as signalR from "@microsoft/signalr"
import { FlightDto } from '../model/FlightDto.model';


@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  public updateTable: EventEmitter<FlightDto[]> = new EventEmitter<FlightDto[]>();
  public data: FlightDto[];
  // public bradcastedData: FlightDto[];

  private hubConnection: signalR.HubConnection
    public startConnection = () => {
      this.hubConnection = new signalR.HubConnectionBuilder()
                              .withUrl('https://localhost:7088/board')
                              .build();
                              
      this.hubConnection
        .start()
        .then(() => console.log('Connection started'))
        .catch(err => console.log('Error while starting connection: ' + err))
    }
    
    public addTransferFlightDataListener = () => {
      this.hubConnection.on('TransferFlightsData', (data) => {
        this.data = data;
        console.log("Just about to emit");
        this.updateTable.emit(data);
        console.log(data);
      });
    }

    
}

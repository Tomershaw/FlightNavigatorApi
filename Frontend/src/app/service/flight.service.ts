import { Injectable } from '@angular/core';
import { HttpClient, HttpClientModule} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class FlightService {

  constructor(private httpClient:HttpClient) { }

  get(){
    console.log('tomer')
   let data = this.httpClient.get('https://localhost:7088/api/ApiControllerFlights')
   console.log(data)
   return data
  }


}

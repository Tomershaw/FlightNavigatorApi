import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { Angular2SmartTableModule } from 'angular2-smart-table';
import { MyTableModule } from './my-table/my-table.module';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ExponentPipe } from './pipes/exponent.pipe';

@NgModule({
  declarations: [AppComponent, ExponentPipe],
  imports: [
    BrowserModule,
    Angular2SmartTableModule,
    MyTableModule,    
    HttpClientModule,
    FormsModule
  ],


    
  providers:[],
  bootstrap: [AppComponent],

})
export class AppModule {}

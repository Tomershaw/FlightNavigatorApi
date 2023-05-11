import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Angular2SmartTableModule } from 'angular2-smart-table';
import { MyTableComponent } from './my-table.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    MyTableComponent,
    MyTableComponent
  ],
  imports: [
    CommonModule,
    Angular2SmartTableModule,
    FormsModule
  ],
  providers: [],
  exports: [MyTableComponent]
  
})
export class MyTableModule { }

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { FAppointmentComponent } from './components/fappointment/fappointment.component';
import { Fappointment2Component } from './components/fappointment2/fappointment2.component';
import { FapptRowComponent } from './components/fappt-row/fappt-row.component';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';

@NgModule({
  declarations: [
    AppComponent,
    FAppointmentComponent,
    Fappointment2Component,
    FapptRowComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

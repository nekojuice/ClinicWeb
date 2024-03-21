import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-fappointment',
  templateUrl: './fappointment.component.html',
  styleUrls: ['./fappointment.component.css']
})
export class FAppointmentComponent {
  constructor(private Client: HttpClient) { }

  dataInput: any;
  ngOnInit() {
    this.Client.get('https://localhost:7071/FAppointment/Get_DeptDoctorInfo/婦產科')
      .subscribe(data => {
        this.dataInput = data;
      })
  }

}

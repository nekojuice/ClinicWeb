import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-fappointment2',
  templateUrl: './fappointment2.component.html',
  styleUrls: ['./fappointment2.component.css']
})
export class Fappointment2Component {
  constructor(private Client: HttpClient) { }

  dataInput: any;
  ngOnInit() {
    this.Client.get('https://localhost:7071/FAppointment/Get_DeptDoctorInfo/小兒科')
      .subscribe(data => {
        this.dataInput = data;
      })
  }
}

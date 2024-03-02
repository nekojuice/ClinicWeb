import { HttpClient } from '@angular/common/http';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-fappt-row',
  templateUrl: './fappt-row.component.html',
  styleUrls: ['./fappt-row.component.css']
})
export class FapptRowComponent {
  constructor(private Client: HttpClient) { }
  @Input() dataInput: any;

  todayDate: Date = new Date();

  weekPage: number = 0;

  dateObjectArr: dateObject[] = [];

  clinicDataObject: any;

  ngOnInit() {
    this.weekChange(0)
  }

  weekChange(direction: number) {
    this.weekPage += direction;
    this.todayDate = new Date(this.todayDate.getTime() + direction * 7 * 24 * 60 * 60 * 1000);
    //let todayDateString = `${this.todayDate.getFullYear()}/${(this.todayDate.getMonth() + 1).toString().padStart(2, '0')}/${(this.todayDate.getDate()).toString().padStart(2, '0')}`;
    //console.log(todayDateString);
    this.putDate()

    this.Client.get(`https://localhost:7071/FAppointment/Get_ClinicApptInfo/${this.dataInput.empId}/${this.todayDate.getFullYear()}/${(this.todayDate.getMonth() + 1).toString().padStart(2, '0')}/${(this.todayDate.getDate()).toString().padStart(2, '0')}`)
      .subscribe(data => {
        //console.log(data);
        this.clinicDataObject = data;
      })
  }

  putDate() {
    let firstDay = this.todayDate;
    this.dateObjectArr.length = 0; //清空物件

    for (var i = 0; i < 7; i++) {
      firstDay = new Date(this.todayDate.getTime() + i * 24 * 60 * 60 * 1000);
      this.dateObjectArr.push({
        date: `${firstDay.getFullYear()}/${(firstDay.getMonth() + 1).toString().padStart(2, '0')}/${(firstDay.getDate()).toString().padStart(2, '0')}`,
        week: firstDay.toLocaleString('zh-tw', { weekday: 'long' })
      })
    }
    //console.log(this.dateObjectArr)
  }

  goAppt(apptId:string) {
    console.log(apptId)
    //串接會員id及apptId 執行掛號
    //進度在此
  }

}

export class dateObject {
  date!: string;
  week!: string;
}

export class clinicInfo {
  docName!: string
  empId!: number
  empPhoto!: BigInt64Array
}

export class clinicDataObject {
  clinicInfoId!: number
  count!: number
  date!: string
  limit!: number
  shift!: string
}

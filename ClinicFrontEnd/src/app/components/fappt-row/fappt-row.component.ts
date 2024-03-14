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
  //todayDate: Date = new Date('2024/2/1'); //暫時寫死

  weekPage: number = 0;

  dateObjectArr: dateObject[] = [];
  shiftObjectArr: any[] = ['早診','午診','晚診'];

  clinicDataObject: any;
  clinicDataArray: any =
    [
      [{}, {}, {}, {}, {}, {}, {}],
      [{}, {}, {}, {}, {}, {}, {}],
      [{}, {}, {}, {}, {}, {}, {}]
    ];

  ngOnInit() {
    this.weekChange(0)
  }

  weekChange(direction: number) {
    this.weekPage += direction;
    this.todayDate = new Date(this.todayDate.getTime() + direction * 7 * 24 * 60 * 60 * 1000);
    //let todayDateString = `${this.todayDate.getFullYear()}/${(this.todayDate.getMonth() + 1).toString().padStart(2, '0')}/${(this.todayDate.getDate()).toString().padStart(2, '0')}`;
    //console.log(todayDateString);

    //放入動態日期
    this.putDate()

    //ajax
    this.Client.get(`https://localhost:7071/FAppointment/Get_ClinicApptInfo/${this.dataInput.empId}/${this.todayDate.getFullYear()}/${(this.todayDate.getMonth() + 1).toString().padStart(2, '0')}/${(this.todayDate.getDate()).toString().padStart(2, '0')}`)
      .subscribe(data => {
        console.log(data);
        this.clinicDataObject = data; //舊的物件集
        this.RearrangeData(data);
        console.log(this.clinicDataArray);
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

  //依規則填入二維陣列
  RearrangeData(apiDatas:any) {
    for (let i = 0; i < apiDatas.length; i++) {
      // 取得星期 Y
      let colIndex = this.getColIndex(this.dateObjectArr, apiDatas[i]);
      // 取得班別  X
      let rowIndex = this.getRowIndex(apiDatas[i]);

      this.clinicDataArray[rowIndex][colIndex] = apiDatas[i];
    }
  }

  getRowIndex(apiData: clinicDataObject) {
    switch (apiData.shift) {
      case '早診':
        return 0;

      case '午診':
        return 1;

      case '晚診':
        return 2;

      default:
        return 0;
    }
  }

  getColIndex(DateHeader: dateObject[], apiData: clinicDataObject) {
    return DateHeader.findIndex(x => x.date === apiData.date);
  }

  getClinicDataDetail(index: number) {
    return this.clinicDataArray[index];
  }

  getShiftDetail(index: number) {
    return this.shiftObjectArr[index];
  }

  goAppt(apptId: string) {
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

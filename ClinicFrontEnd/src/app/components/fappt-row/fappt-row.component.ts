import { HttpClient } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-fappt-row',
  templateUrl: './fappt-row.component.html',
  styleUrls: ['./fappt-row.component.css']
})
export class FapptRowComponent {
  constructor(private Client: HttpClient, private activatdeRoute: ActivatedRoute) { }
  memid = this.activatdeRoute.snapshot.queryParamMap.get('id');
  @Input() dataInput: any;

  //todayDate: Date = new Date();
  todayDate: Date = new Date('2023/12/01'); //暫時寫死

  weekPage: number = 0;

  dateObjectArr: dateObject[] = [];
  shiftObjectArr: any[] = ['早診', '午診', '晚診'];

  //clinicDataObject: any;
  clinicDataArray: any =
    [
      [{}, {}, {}, {}, {}, {}, {}],
      [{}, {}, {}, {}, {}, {}, {}],
      [{}, {}, {}, {}, {}, {}, {}]
    ];

  ngOnInit() {
    this.weekChange(0)
    //console.log(this.memid)
  }

  weekChange(direction: number) {
    this.weekPage += direction;
    this.todayDate = new Date(this.todayDate.getTime() + direction * 7 * 24 * 60 * 60 * 1000);
    //let todayDateString = `${this.todayDate.getFullYear()}/${(this.todayDate.getMonth() + 1).toString().padStart(2, '0')}/${(this.todayDate.getDate()).toString().padStart(2, '0')}`;
    //console.log(todayDateString);

    //放入動態日期
    this.putDate()
    //ajax
    this.ajaxData()
  }

  ajaxData() {
    let url = `https://localhost:7071/FAppointment/Get_ClinicApptInfo/${this.dataInput.empId}/${this.todayDate.getFullYear()}/${(this.todayDate.getMonth() + 1).toString().padStart(2, '0')}/${(this.todayDate.getDate()).toString().padStart(2, '0')}`;
    if (this.memid) {
      url += `/${this.memid}`;
    }
    this.Client.get(url)
      .subscribe(data => {
        //console.log(data);
        //this.clinicDataObject = data; //舊的物件集
        this.RearrangeData(data);
        //console.log(this.clinicDataArray);
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
  RearrangeData(apiDatas: any) {
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

  goAppt(clinicdata: clinicDataObject) {
    //console.log(clinicdata.clinicInfoId)
    if (!this.memid) {
      alert('未登入時僅供查詢!\n若欲掛號，請先登入會員')
      return
    }
    let body = {
      "apptId": +clinicdata.clinicInfoId,
      "memid": +this.memid
    }
    this.Client.post(`https://localhost:7071/FAppointment/Add_NewAppt`, body)
      .subscribe(data => {
        //console.log(data)
        alert(`掛號成功: ${this.dataInput.docName} 醫師, ${clinicdata.date}, ${clinicdata.shift}`)
        this.ajaxData()
      })
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
  isAppted!: boolean
}

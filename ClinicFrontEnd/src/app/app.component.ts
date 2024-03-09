import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {



  title = 'ClinicFrontEnd';

  deptSwitch: number = 0;

  changeDepartment(dept:number) {
    this.deptSwitch = dept;
  }


}

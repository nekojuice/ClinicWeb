import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  constructor(private Client: HttpClient, private activatdeRoute: ActivatedRoute) { }
  //memid = this.activatdeRoute.snapshot.queryParamMap.get('id');
  memid = new URLSearchParams(window.location.search).get('id');
  memName: any = ''

  ngOnInit() {
    //console.log(this.memid)
    if (!this.memid) {
      return
    }

    this.Client.get(`https://localhost:7071/FAppointment/Get_MemberName/${this.memid}`)
      .subscribe(data => {
        this.memName = data
        //console.log(this.memName)
      })
  }
}

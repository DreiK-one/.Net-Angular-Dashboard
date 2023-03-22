import { AuthService } from './../../../services/auth.service';
import { ApiService } from './../../../services/api.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-section-users',
  templateUrl: './section-users.component.html',
  styleUrls: ['./section-users.component.css']
})
export class SectionUsersComponent implements OnInit {

  public users: any = [];

  constructor(private _apiService: ApiService) { }

  ngOnInit(): void {
    this._apiService.getUsers().subscribe(res => {
      this.users = res;
    })
  }

}

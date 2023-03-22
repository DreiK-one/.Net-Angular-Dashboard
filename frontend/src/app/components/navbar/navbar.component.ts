import { UserStoreService } from './../../services/user-store.service';
import { AuthService } from './../../services/auth.service';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  public fullName: string = "";

  constructor(private _authService: AuthService, 
    private _userStore: UserStoreService) { }

  ngOnInit(): void {
    this._userStore.getFullNameFromStore().subscribe(res => {
      let fullnameFromToken = this._authService.getFullnameFromToken();
      this.fullName = res || fullnameFromToken;
    });
  }

  logout(){
    this._authService.signout();
  }

}

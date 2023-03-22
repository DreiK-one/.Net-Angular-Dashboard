import { AuthService } from './../../services/auth.service';
import { UserStoreService } from './../../services/user-store.service';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {

  public role: string = "";

  constructor(private _userStore: UserStoreService, 
    private _authService: AuthService) { }

  ngOnInit(): void {
    this._userStore.getRoleFromStore().subscribe(res => {
      const roleFromToken = this._authService.getRoleFromToken();
      this.role = res || roleFromToken;
    })
  }

}

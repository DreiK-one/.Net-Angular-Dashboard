import { UserStoreService } from './../../services/user-store.service';
import { AuthService } from './../../services/auth.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import ValidateForm from 'src/app/helpers/validateform';
import { Login } from 'src/app/shared/login.model';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  loginForm!: FormGroup;

  constructor(private _formBuilder: FormBuilder, 
    private _authService: AuthService, 
    private _router: Router,
    private _toast: NgToastService, 
    private _userStore: UserStoreService) { }

  ngOnInit(): void {
    this.loginForm = this._formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? ((this.eyeIcon = "fa-eye") && (this.type = "text")) : 
        ((this.eyeIcon = "fa-eye-slash") && (this.type = "password"))
  }

  async onLogin(){
    if(this.loginForm.valid){
      await this._authService.login(this.loginForm.value as Login).subscribe({
        next: (res) => {
          this.loginForm.reset();
          this._authService.storeToken(res.accessToken);
          const tokenPayload = this._authService.decodeToken();
          this._userStore.setFullNameForStore(tokenPayload.unique_name);
          this._userStore.setRoleForStore(tokenPayload.role);
          this._toast.success({detail: "SUCCESS", summary: res.message, duration: 5000});
          this._router.navigate(['sales']);
        },
        error: (err) => {
          this._toast.error({detail: "ERROR", summary: err.error.message, duration: 5000});
        }
      });
    }else{
      ValidateForm.validateAllFormFields(this.loginForm);
      this._toast.error({detail: "ERROR", summary: "Your form is invalid", duration: 5000});
    }
  }
}

import { ResetPasswordService } from './../../services/reset-password.service';
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
  public resetPasswordEmail!: string;
  public isValidEmail!: boolean;

  constructor(private _formBuilder: FormBuilder, 
    private _authService: AuthService, 
    private _router: Router,
    private _toast: NgToastService, 
    private _userStore: UserStoreService,
    private _resetPasswordService: ResetPasswordService) { }

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
          this._authService.storeRefreshToken(res.refhreshToken);
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

  checkValidEmail(event: string){
    const value = event;
    const pattern = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;
    this.isValidEmail = pattern.test(value);
    return this.isValidEmail;
  }

  confirmToSend(){
    if (this.checkValidEmail(this.resetPasswordEmail)) {
      console.log(this.resetPasswordEmail);

      this._resetPasswordService
        .sendResetPasswordLink(this.resetPasswordEmail)
        .subscribe({
          next: (res) => {
            this._toast.success({detail: "SUCCESS", summary: "Reset success!", duration: 3000});
            this.resetPasswordEmail = "";
            const buttonRef = document.getElementById("closeBtn");
            buttonRef?.click();
          },
          error: (err) =>{
            this._toast.error({detail: "ERROR", summary: err.error.message, duration: 5000});
          }
        })
    }
  }
}

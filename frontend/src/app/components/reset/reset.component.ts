import { NgToastService } from 'ng-angular-popup';
import { ResetPasswordService } from './../../services/reset-password.service';
import ValidateForm from 'src/app/helpers/validateform';
import { ResetPasswordModel } from './../../shared/reset-password.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { ConfirmPasswordValidator } from 'src/app/helpers/confirm-password.validator';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-reset',
  templateUrl: './reset.component.html',
  styleUrls: ['./reset.component.css']
})
export class ResetComponent implements OnInit {

  resetPasswordForm!: FormGroup;
  emailToReset!: string;
  emailToken!: string;
  resetPasswordObject = new ResetPasswordModel();

  constructor(private _formBuilder: FormBuilder,
    private _activatedRoute: ActivatedRoute,
    private _resetPasswordService: ResetPasswordService, 
    private _toast: NgToastService,
    private _router: Router) { }

  ngOnInit(): void {
    this.resetPasswordForm = this._formBuilder.group({
      password: [null, Validators.required],
      confirmPassword: [null, Validators.required]
    }, {
      validator: ConfirmPasswordValidator("password", "confirmPassword")
    });

    this._activatedRoute.queryParams.subscribe(value => {
      this.emailToReset = value['email'];
      let uriToken = value['code'];
      this.emailToken = uriToken.replace(/ /g, '+');
    })
  }

  reset(){
    if (this.resetPasswordForm.valid) {
      this.resetPasswordObject.email = this.emailToReset;
      this.resetPasswordObject.newPassword = this.resetPasswordForm.value.password;
      this.resetPasswordObject.confirmPassword = this.resetPasswordForm.value.confirmPassword;
      this.resetPasswordObject.emailToken = this.emailToken;

      this._resetPasswordService.resetPassword(this.resetPasswordObject)
      .subscribe({
        next: (res) => {
          this._toast.success({
            detail: 'SUCCESS',
            summary: 'Password reset successfully!',
            duration: 3000
          });
          this._router.navigate(['login']);
        },
        error: (err) => {
          this._toast.error({
            detail: 'ERROR',
            summary: 'Something went wrong!',
            duration: 3000
          });
        }
      })
    }else{
      ValidateForm.validateAllFormFields(this.resetPasswordForm);
    }
  }

}

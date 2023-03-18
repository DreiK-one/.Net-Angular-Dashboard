import { NgToastService } from 'ng-angular-popup';
import { User } from './../../shared/user.model';
import { AuthService } from './../../services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import ValidateForm from 'src/app/helpers/validateform';
import { Router } from '@angular/router';


@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  type: string = "password";
  isText: boolean = false;
  eyeIcon: string = "fa-eye-slash";
  signUpForm!: FormGroup;

  constructor(private formBuilder: FormBuilder, 
    private _authService: AuthService, 
    private _router: Router,
    private _toast: NgToastService) { }

  ngOnInit(): void {
    this.signUpForm = this.formBuilder.group({
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      username: ['', Validators.required],
      email: ['', Validators.required],
      password: ['', Validators.required],
    })
  }

  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? ((this.eyeIcon = "fa-eye") && (this.type = "text")) : 
        ((this.eyeIcon = "fa-eye-slash") && (this.type = "password"));
  }

  async onSignUp(){
    if(this.signUpForm.valid){
      await this._authService.signUp(this.signUpForm.value as User).subscribe({
        next: (res) => {
          this.signUpForm.reset();
          this._toast.success({detail: "SUCCESS", summary: res.message, duration: 5000});
          this._router.navigate(['login']);
        },
        error: (err) =>{
          this._toast.error({detail: "ERROR", summary: err.error.message, duration: 5000});
        }
      });
    }else{
      ValidateForm.validateAllFormFields(this.signUpForm);
      this._toast.error({detail: "ERROR", summary: "Your form is invalid", duration: 5000});
    }
  }
}

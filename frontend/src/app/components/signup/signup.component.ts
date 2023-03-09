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
    private _router: Router) { }

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
      console.log(this.signUpForm.value);
      await this._authService.signUp(this.signUpForm.value as User).subscribe({
        next: (res) => {
          alert(res.message);
          this.signUpForm.reset();
          this._router.navigate(['login']);
        },
        error: (err) =>{
          alert(err?.error.message);
        }
      });
    }else{
      ValidateForm.validateAllFormFields(this.signUpForm);
      //logic for throwing error
    }
  }
}

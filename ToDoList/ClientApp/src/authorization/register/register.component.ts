import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthorizeService } from '../authorize.service';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  loading = false;
  submitted = false;
  returnUrl: string;
  error = '';

  constructor(private formBuilder: FormBuilder,
    private authorizeService: AuthorizeService,
    private activatedRoute: ActivatedRoute,
    private router: Router) {
    if (this.authorizeService.currentUserValue) {
      this.router.navigate(['/']);
    }
  }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      username: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, , Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required, , Validators.minLength(8)]]
    }
      //{ validator: this.passwordMatchValidator },
    );

    // get return url from route parameters or default to '/'
    this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'] || '/';
  }
  get f() { return this.registerForm.controls; }

  //passwordMatchValidator(frm: FormGroup) {
  //  return frm.controls['password'].value ===
  //    frm.controls['confirmPassword'].value ? null : { 'mismatch': true };
  //}

  onSubmit() {
    this.submitted = true;
    // stop here if form is invalid
    if (this.registerForm.invalid || this.f.password.value != this.f.confirmPassword.value) {
      return
    }

    this.loading = true;
    this.authorizeService.register(this.f.username.value, this.f.password.value)
      .pipe(first())
      .subscribe(
        data => {
          this.router.navigate([this.returnUrl]);
        },
        error => {
          this.error = error.error.message;
          this.loading = false;
        });
  }

}

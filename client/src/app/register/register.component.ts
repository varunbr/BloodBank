import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  validationErrors: string[] = [];

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router
  ) {
    this.initilizeForm();
  }

  ngOnInit(): void {}

  initilizeForm() {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      userName: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      email: ['', Validators.email],
      dateOfBirth: ['', Validators.required],
      gender: ['male', Validators.required],
      bloodGroup: ['', Validators.required],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(12),
        ],
      ],
      confirmPassword: [
        '',
        [Validators.required, this.matchValues('password')],
      ],
      area: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      country: ['', Validators.required],
      postalCode: ['', Validators.required],
    });

    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    });
  }

  matchValues(name: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      return control?.value === control?.parent?.controls[name]?.value
        ? null
        : { notMatching: true };
    };
  }

  onSubmit() {
    this.accountService.register(this.registerForm.value).subscribe(
      () => {
        this.router.navigateByUrl('/');
      },
      (error) => {
        this.validationErrors = [error.error];
        console.log(error);
      }
    );
  }

  bloodGroupList = [
    {
      name: 'O+',
      value: 'Op',
    },
    {
      name: 'O-',
      value: 'On',
    },
    {
      name: 'A+',
      value: 'Ap',
    },
    {
      name: 'A-',
      value: 'An',
    },
    {
      name: 'B+',
      value: 'Bp',
    },
    {
      name: 'B-',
      value: 'Bn',
    },
    {
      name: 'AB+',
      value: 'ABp',
    },
    {
      name: 'AB-',
      value: 'ABn',
    },
  ];
}

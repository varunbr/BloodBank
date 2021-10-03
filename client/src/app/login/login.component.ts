import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  modal: any = {};
  loggingIn: boolean = false;

  constructor(private accountService: AccountService, private router: Router) {
    const state = this.router.getCurrentNavigation()?.extras?.state;
    if (state?.reload) {
      window.location.reload();
    }
  }

  ngOnInit(): void {}

  onSubmit() {
    this.accountService.login(this.modal).subscribe(
      () => {
        this.router.navigateByUrl('/');
      }
    );
  }
}

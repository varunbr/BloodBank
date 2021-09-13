import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  isCollapsed = true;
  constructor(public accountService: AccountService, public router: Router) {}

  ngOnInit(): void {}

  logout() {
    this.accountService.logout();
  }

  inAuthPage() {
    if (this.router.url === '/login' || this.router.url === '/register') {
      return true;
    }
    return false;
  }
}

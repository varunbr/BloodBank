import { Component, OnInit } from '@angular/core';
import { User } from './_modals/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'Blood Bank App';
  constructor(private accountSerice: AccountService) {}

  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const value = localStorage.getItem('user');
    if (value) {
      const user: User = JSON.parse(value);
      this.accountSerice.setUser(user);
    }
  }
}

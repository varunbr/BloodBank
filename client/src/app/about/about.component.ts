import { Component, OnInit } from '@angular/core';
import { Role } from '../_modals/admin';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css'],
})
export class AboutComponent implements OnInit {
  constructor(private accountService: AccountService) {}

  roles: Role[];
  ngOnInit(): void {
    this.accountService.getRolesForAbout().subscribe((response) => {
      this.roles = response;
    });
  }
}

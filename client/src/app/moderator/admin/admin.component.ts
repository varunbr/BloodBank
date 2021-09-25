import { Component, OnInit } from '@angular/core';
import { Bank } from 'src/app/_modals/bank';
import { BankParams } from 'src/app/_modals/bankParams';
import { AdminService } from 'src/app/_services/admin.service';
import { BaseListComponent } from 'src/app/_services/base-list.component';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css'],
})
export class AdminComponent
  extends BaseListComponent<Bank, BankParams>
  implements OnInit
{
  constructor(adminService: AdminService) {
    super(adminService);
  }

  ngOnInit(): void {
    this.loadModals();
  }
}

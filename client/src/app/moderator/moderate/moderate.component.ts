import { Component, OnInit } from '@angular/core';
import { Bank } from 'src/app/_modals/bank';
import { BankParams } from 'src/app/_modals/bankParams';
import { BaseListComponent } from 'src/app/_services/base-list.component';
import { ModerateService } from 'src/app/_services/moderate.service';

@Component({
  selector: 'app-moderate',
  templateUrl: './moderate.component.html',
  styleUrls: ['./moderate.component.css'],
})
export class ModerateComponent
  extends BaseListComponent<Bank, BankParams>
  implements OnInit
{
  constructor(moderateService: ModerateService) {
    super(moderateService);
  }

  ngOnInit(): void {
    this.loadModals();
  }
}

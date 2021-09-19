import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Bank } from 'src/app/_modals/bank';
import { BankParams } from 'src/app/_modals/bankParams';
import { BankService } from 'src/app/_services/bank.service';
import { BaseListComponent } from 'src/app/_services/base-list.component';

@Component({
  selector: 'app-banks-list',
  templateUrl: './banks-list.component.html',
  styleUrls: ['./banks-list.component.css'],
})
export class BanksListComponent
  extends BaseListComponent<Bank, BankParams>
  implements OnInit
{
  modalRef?: BsModalRef;
  constructor(
    bankService: BankService,
    private bsmodalService: BsModalService
  ) {
    super(bankService);
  }

  ngOnInit(): void {
    this.loadModals();
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.bsmodalService.show(template, {});
  }
}

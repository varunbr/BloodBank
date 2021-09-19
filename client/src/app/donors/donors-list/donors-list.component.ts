import { Component, OnInit, TemplateRef } from '@angular/core';
import { Donor } from 'src/app/_modals/donor';
import { DonorParams } from 'src/app/_modals/donorParams';
import { DonorService } from 'src/app/_services/donor.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { BaseListComponent } from 'src/app/_services/base-list.component';

@Component({
  selector: 'app-donors-list',
  templateUrl: './donors-list.component.html',
  styleUrls: ['./donors-list.component.css'],
})
export class DonorsListComponent
  extends BaseListComponent<Donor, DonorParams>
  implements OnInit
{
  modalRef?: BsModalRef;

  constructor(
    donorService: DonorService,
    private bsmodalService: BsModalService
  ) {
    super(donorService);
  }

  ngOnInit() {
    this.loadModals();
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.bsmodalService.show(template, {});
  }
}

import { Component, OnInit, TemplateRef } from '@angular/core';
import { Donor } from 'src/app/_modals/donor';
import { DonorParams } from 'src/app/_modals/donorParams';
import { Pagination } from 'src/app/_modals/pagination';
import { getBloodGroupList } from 'src/app/_modals/utility';
import { DonorService } from 'src/app/_services/donor.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-donors-list',
  templateUrl: './donors-list.component.html',
  styleUrls: ['./donors-list.component.css'],
})
export class DonorsListComponent implements OnInit {
  donors: Donor[] = [];
  params: DonorParams;
  pagination: Pagination;
  bloodGroupList = getBloodGroupList();
  addressPlaceholderItems = ['Area', 'City', 'Postal Code', 'State', 'Country'];
  addressCurrentIndex = this.addressPlaceholderItems.length;
  addressPlaceholder = this.addressPlaceholderItems[0];
  modalRef?: BsModalRef;

  constructor(
    private donorService: DonorService,
    private modalService: BsModalService
  ) {
    this.params = this.donorService.getDonorParams();
  }

  ngOnInit() {
    setInterval(() => {
      this.addressCurrentIndex++;
      if (this.addressCurrentIndex >= this.addressPlaceholderItems.length) {
        this.addressCurrentIndex = 0;
      }
      this.addressPlaceholder =
        this.addressPlaceholderItems[this.addressCurrentIndex];
    }, 3000);

    this.loadDonors();
  }

  loadDonors() {
    this.donorService.getDonors().subscribe((response) => {
      this.donors = response.result;
      this.pagination = response.pagination;
    });
  }

  applyFilter() {
    this.loadDonors();
  }

  resetFilter() {
    this.donorService.resetDonorParams();
    this.params = this.donorService.getDonorParams();
    this.loadDonors();
  }

  pageChanged(event) {
    this.params.pageNumber = event.page;
    this.loadDonors();
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, {});
  }
}

import { BaseModal } from '../_modals/modal';
import { PageParams } from '../_modals/pageParams';
import { Pagination } from '../_modals/pagination';
import { getBloodGroupList } from '../_modals/utility';
import { BasePageService } from './base-page.service';

export abstract class BaseListComponent<Modal extends BaseModal, Param extends PageParams> {
  modals: Modal[] = [];
  params: Param;
  pagination: Pagination;
  bloodGroupList = getBloodGroupList();
  addressPlaceholderItems = ['Area', 'City', 'Postal Code', 'State', 'Country'];
  addressCurrentIndex = this.addressPlaceholderItems.length;
  addressPlaceholder = this.addressPlaceholderItems[0];

  constructor(private modalService: BasePageService<Modal, Param>) {
    this.params = modalService.getParams();
    setInterval(() => {
      this.addressCurrentIndex++;
      if (this.addressCurrentIndex >= this.addressPlaceholderItems.length) {
        this.addressCurrentIndex = 0;
      }
      this.addressPlaceholder =
        this.addressPlaceholderItems[this.addressCurrentIndex];
    }, 3000);
  }

  loadModals() {
    this.modalService.getModals().subscribe((response) => {
      this.modals = response.result;
      this.pagination = response.pagination;
    });
  }

  applyFilter() {
    this.loadModals();
  }

  resetFilter() {
    this.modalService.resetParams();
    this.params = this.modalService.getParams();
    this.loadModals();
  }

  pageChanged(event) {
    this.params.pageNumber = event.page;
    this.loadModals();
  }
}

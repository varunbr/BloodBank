import { PageParams } from './pageParams';

export class DonorParams extends PageParams {
  gender: string = '';
  maxAge = 50;
  minAge = 18;
  bloodGroup: string = '';
  address: string = '';
  orderBy: string = '';
}

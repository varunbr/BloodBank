import { BaseModal } from './modal';

export interface Donor extends BaseModal {
  name: string;
  userName: string;
  photoUrl: string;
  bloodGroup: string;
  age: number;
  phoneNumber: string;
  email: string;
  gender: string;
  lastActive: string;
  area: string;
  city: string;
  state: string;
  country: string;
  postalCode: string;
}

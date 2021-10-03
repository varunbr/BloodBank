import { Role } from './admin';
import { BaseModal } from './modal';

export interface Bank extends BaseModal {
  name: string;
  photoUrl: string;
  phoneNumber: string;
  email: string;
  website: string;
  lastUpdated: string;
  area: string;
  city: string;
  state: string;
  country: string;
  postalCode: string;
  bloodGroups: BloodGroup[];
  role: string;
  moderators: Role[];
}

export interface BloodGroup {
  group: string;
  value: string;
}

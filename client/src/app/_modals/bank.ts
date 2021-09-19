export interface Bank {
  id: number;
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
}

export interface BloodGroup {
  group: string;
  value: string;
}

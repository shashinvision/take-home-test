export interface Loan {
  id: number;
  amount: number;
  currentBalance: number;
  isActive: number;
  createdAt: string;
  updateAt: string;
  idApplicant: number;
  idApplicantNavigation: IdApplicantNavigation;
  payments?: PaymentsEntity[] | null;
}
export interface IdApplicantNavigation {
  id: number;
  fullName: string;
  dni: string;
}
export interface PaymentsEntity {
  id: number;
  amount: number;
  idLoan: number;
  createdAt: string;
}

import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { MatTableModule } from "@angular/material/table";
import { MatButtonModule } from "@angular/material/button";
import { Loan } from "./_models/loan";
import { LoanService } from "./_services/loan.service";

@Component({
  selector: "app-root",
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule],
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"],
})
export class AppComponent implements OnInit {
  loans: Loan[] = [];

  displayedColumns: string[] = [
    "loanAmount",
    "currentBalance",
    "applicant",
    "status",
  ];
  constructor(private loanservice: LoanService) {}

  ngOnInit() {
    this.loanservice.getLoans().subscribe((loans) => {
      this.loans = loans;
    });
  }
}

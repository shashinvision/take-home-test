import { Component, inject } from "@angular/core";
import { CommonModule } from "@angular/common";
import { TableComponent } from "../../_components/table/table.component";
import { AuthService } from "../../_services/auth.service";

@Component({
  selector: "app-home",
  standalone: true,
  imports: [CommonModule, TableComponent],
  templateUrl: "./home.component.html",
  styleUrl: "./home.component.scss",
})
export class HomeComponent {
  private authService = inject(AuthService);

  logout(): void {
    this.authService.logout();
  }
}

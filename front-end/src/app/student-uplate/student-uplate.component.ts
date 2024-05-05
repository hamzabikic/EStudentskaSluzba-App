import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../MojConfig";
import {MyAuthService} from "../Services/MyAuthService";

@Component({
  selector: 'app-student-uplate',
  templateUrl: './student-uplate.component.html',
  styleUrls: ['./student-uplate.component.css']
})
export class StudentUplateComponent implements OnInit {
  studentId:number =0;
  student:any;
  lista:any;
  noviUpis = false;
  datumUpisa ="";
  upisanaGodina ="";
  obnova:boolean = false;
  novaOvjera = false;
  datumOvjere ="";
  upisId="";
  constructor(private route: ActivatedRoute, private http:HttpClient, private myAuth: MyAuthService) {
    this.studentId = this.route.snapshot.params["id"];

  }

  async ngOnInit() {
    await this.ucitajStudenta();
    await this.ucitajUplate();
    this.setujDatum();
  }
  setujDatum() {
    this.datumUpisa=new Date().toISOString().split('T')[0];
    this.datumOvjere = this.datumUpisa;
  }
  async ucitajStudenta() {
    this.student = await this.http.get(MojConfig.adresa_servera +"/Student/getById?id=" + this.studentId).toPromise();
  }
  async ucitajUplate() {
    this.lista = await this.http.get(MojConfig.adresa_servera + "/Upisi/getUpisiById?studentid="+ this.studentId).toPromise();
  }
  async posaljiUpis() {
     let obj = {
       studentId: this.studentId,
       referentId: this.myAuth.getId(),
       datumUpisa:this.datumUpisa,
       upisanaGodina:this.upisanaGodina,
       obnova: this.obnova
     };
     let res = await this.http.post(MojConfig.adresa_servera + "/Upisi/addUpis", obj).toPromise();
     // @ts-ignore
     if(res.upisan) {
       alert("Uspjesno dodan upis!");
       await this.ucitajUplate();
       this.noviUpis = !this.noviUpis;
       return;
     }
     // @ts-ignore
     alert(res.greska);
  }
  async obrisiUpis(id:number) {
    let res = await this.http.delete(MojConfig.adresa_servera+"/Upisi/deleteUpis?upisId="+id).toPromise();
    if(res == true) {
      alert ("Uspjesno brisanje!");
      await this.ucitajUplate();
      return;
    }
    alert("Neuspjesno brisanje!");
  }
  async ovjeri () {
    let obj = {
      upisId: this.upisId,
      datumOvjere : this.datumOvjere
    };
    let res = await this.http.patch(MojConfig.adresa_servera+"/Upisi/ovjera", obj).toPromise();
    if(res == true) {
      alert("Uspjesna ovjera!");
      await this.ucitajUplate();
      this.novaOvjera =!this.novaOvjera;
      return;
    }
    alert("Neuspjesna ovjera!");
  }


}

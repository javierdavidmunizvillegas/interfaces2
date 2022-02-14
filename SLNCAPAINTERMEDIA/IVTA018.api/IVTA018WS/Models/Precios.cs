using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace IVTA018WS.Models
{
    public partial class Precios : DbContext
    {
        public Precios()
            : base("name=Precios2")
        {
        }

        public virtual DbSet<ContiParametro> ContiParametro { get; set; }
        public virtual DbSet<FlintFoxProductWork> FlintFoxProductWork { get; set; }
        public virtual DbSet<FlintFoxPVPMax> FlintFoxPVPMax { get; set; }
        public virtual DbSet<ContiProceso> ContiProceso { get; set; }
        public virtual DbSet<VW_ContiGenPClasificador> VW_ContiGenPClasificador { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContiParametro>()
                .Property(e => e.CIDGRUPOPARAMETRO)
                .IsUnicode(false);

            modelBuilder.Entity<ContiParametro>()
                .Property(e => e.CIDPARAMETRO)
                .IsUnicode(false);

            modelBuilder.Entity<ContiParametro>()
                .Property(e => e.CDESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<ContiParametro>()
                .Property(e => e.CVALOR)
                .IsUnicode(false);

            modelBuilder.Entity<ContiParametro>()
                .Property(e => e.CESTADOREGISTRO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ContiParametro>()
                .Property(e => e.CUSUARIOINGRESO)
                .IsUnicode(false);

            modelBuilder.Entity<ContiParametro>()
                .Property(e => e.CTERMINALINGRESO)
                .IsUnicode(false);

            modelBuilder.Entity<ContiParametro>()
                .Property(e => e.CUSUARIOMODIFICACION)
                .IsUnicode(false);

            modelBuilder.Entity<ContiParametro>()
                .Property(e => e.CTERMINALMODIFICACION)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.IdProd)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.Empresa)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.Canal)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.Cliente)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.CalificacionCliente)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.Plazo)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.CodProducto)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.EstadoInventario)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.NumSerie)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.AttString)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.NumAcuerdoCosto)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionCosto)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.ValorTotalCosto)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.PorcentajeCosto)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.CodErrorCosto)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionErrorCosto)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.NumAcuerdoIndCom)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionIndCom)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.ValorTotalIndCom)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.PorcentajeIndCom)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.CodErrorIndCom)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionErrorIndCom)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.NumAcuerdoRecAntPO)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionRecAntPO)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.ValorTotalRecAntPO)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.PorcentajeRecAntPO)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.CodErrorRecAntPO)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionErrorRecAntPO)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.NumAcuerdoPO)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionPO)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.ValorTotalPO)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.PorcentajePO)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.CodErrorPO)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionErrorPO)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.NumAcuerdoIndFinM)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionIndFinM)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.ValorTotalIndFinM)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.PorcentajeIndFinM)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.CodErrorIndFinM)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionErrorIndFinM)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.NumAcuerdoPVPAAlc)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionPVPAAlc)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.ValorTotalPVPAAlc)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.PorcentajePVPAAlc)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.CodErrorPVPAAlc)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionErrorPVPAAlc)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.NumAcuerdoIndAlc)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionIndAlc)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.ValorTotalIndAlc)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.PorcentajeIndAlc)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.CodErrorIndAlc)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionErrorIndAlc)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.NumAcuerdoPVPMax)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionPVPMax)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.ValorTotalPVPMax)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.PorcentajePVPMax)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.CodErrorPVPMax)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionErrorPVPMax)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.NumAcuerdoDescPP)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionDescPPx)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.ValorTotalDescPP)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.PorcentajeDescPP)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.CodErrorDescPP)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionErrorDescPP)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.NumAcuerdoDesWeb)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionDesWeb)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.ValorTotalDesWeb)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.PorcentajeDesWeb)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.CodErrorDesWeb)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionErrorDesWeb)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.NumAcuerdoPVP)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionPVP)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.ValorTotalPVP)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.PorcentajePVP)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.CodErrorPVP)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionErrorPVP)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.NumAcuerdoCuotaIni)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionCuotaIni)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.ValorTotalCuotaIni)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.PorcentajeCuotaIni)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.CodErrorCuotaIni)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.DescripcionErrorCuotaIni)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.UsuarioActualizacion)
                .IsUnicode(false);

            modelBuilder.Entity<FlintFoxPVPMax>()
                .Property(e => e.IpActualizacion)
                .IsUnicode(false);

            modelBuilder.Entity<ContiProceso>()
                .Property(e => e.NombreProceso)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ContiGenPClasificador>()
                .Property(e => e.CCODIGOMODULO)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ContiGenPClasificador>()
                .Property(e => e.CCODIGOGRUPO)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ContiGenPClasificador>()
                .Property(e => e.CCODIGO)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ContiGenPClasificador>()
                .Property(e => e.CESTADOREGISTRO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VW_ContiGenPClasificador>()
                .Property(e => e.CDESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ContiGenPClasificador>()
                .Property(e => e.CPREFIJO)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ContiGenPClasificador>()
                .Property(e => e.CDESCRIPCIONCORTA)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ContiGenPClasificador>()
                .Property(e => e.CCODIGOAUTOMATICO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VW_ContiGenPClasificador>()
                .Property(e => e.CUSUARIOINGRESO)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ContiGenPClasificador>()
                .Property(e => e.CTERMINALINGRESO)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ContiGenPClasificador>()
                .Property(e => e.CUSUARIOMODIFICACION)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ContiGenPClasificador>()
                .Property(e => e.CTERMINALMODIFICACION)
                .IsUnicode(false);
        }
    }
}

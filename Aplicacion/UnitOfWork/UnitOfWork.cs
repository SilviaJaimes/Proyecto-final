using Aplicacion.Repository;
using Dominio.Entities;
using Dominio.Interfaces;
using Persistencia;

namespace Aplicacion.UnitOfWork;
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApiContext _context;
    private RolRepository _roles;
    private UsuarioRepository _usuarios;
    private ClienteRepository _clientes;
    private DetallePedidoRepository _detallePedidos;
    private EmpleadoRepository _empleados;
    private GamaProductoRepository _gamaProductos;
    private OficinaRepository _oficinas;
    private PagoRepository _pagos;
    private PedidoRepository _pedidos;
    private ProductoRepository _productos;

    public UnitOfWork(ApiContext context)
    {
        _context = context;
    }

    public IRol Roles
    {
        get
        {
            if (_roles == null)
            {
                _roles = new RolRepository(_context);
            }
            return _roles;
        }
    }

    public IUsuario Usuarios
    {
        get
        {
            if (_usuarios == null)
            {
                _usuarios = new UsuarioRepository(_context);
            }
            return _usuarios;
        }
    }

    public ICliente Clientes
    {
        get
        {
            if (_clientes == null)
            {
                _clientes = new ClienteRepository(_context);
            }
            return _clientes;
        }
    }

    public IDetallePedido DetallePedidos 
    {
        get
        {
            if (_detallePedidos == null)
            {
                _detallePedidos = new DetallePedidoRepository(_context);
            }
            return _detallePedidos;
        }
    }

    public IEmpleado Empleados
    {
        get
        {
            if (_empleados == null)
            {
                _empleados = new EmpleadoRepository(_context);
            }
            return _empleados;
        }
    }

    public IGamaProducto GamaProductos 
    {
        get
        {
            if (_gamaProductos == null)
            {
                _gamaProductos = new GamaProductoRepository(_context);
            }
            return _gamaProductos;
        }
    }

    public IOficina Oficinas
    {
        get
        {
            if (_oficinas == null)
            {
                _oficinas = new OficinaRepository(_context);
            }
            return _oficinas;
        }
    }

    public IPago Pagos
    {
        get
        {
            if (_pagos == null)
            {
                _pagos = new PagoRepository(_context);
            }
            return _pagos;
        }
    }

    public IPedido Pedidos
    {
        get
        {
            if (_pedidos == null)
            {
                _pedidos = new PedidoRepository(_context);
            }
            return _pedidos;
        }
    }

    public IProducto Productos
    {
        get
        {
            if (_productos == null)
            {
                _productos = new ProductoRepository(_context);
            }
            return _productos;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
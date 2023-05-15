using AutoMapper;
using Data.Entities;
using Business.Models;

namespace Business
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile() 
        {
            CreateMap<UserStatus, UserStatusModel>().ReverseMap();
            CreateMap<StorageType, StorageTypeModel>().ReverseMap();
            CreateMap<OrderStatus, OrderStatusModel>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailModel>().ReverseMap();
            CreateMap<ReceiptDetail, ReceiptDetailModel>().ReverseMap();
            CreateMap<Receipt, ReceiptModel>()
                .ForMember(receiptModel => receiptModel.ReceiptDetailsIds,
                receipt => receipt.MapFrom(x => x.ReceiptDetails.Select(receiptDetails => receiptDetails.Id)))
                .ReverseMap();
            CreateMap<Product, ProductModel>()
                .ForMember(productModel=>productModel.StorageTypeName,
                product=>product.MapFrom(x=>x.StorageType.StorageTypeName))
                .ReverseMap();
            CreateMap<Order, OrderModel>()
                .ForMember(orderModel => orderModel.OrderStatusName,
                order => order.MapFrom(x => x.OrderStatus.OrderStatusName))
                .ForMember(orderModel => orderModel.OrderDetailsIds,
                order=>order.MapFrom(x=>x.OrderDetails.Select(orderDetail=> orderDetail.Id)))
                .ReverseMap();
            CreateMap<User, UserModel>()
                .ForMember(userModel => userModel.UserStatusName,
                user => user.MapFrom(x => x.UserStatus.UserStatusName))
                .ForMember(userModel => userModel.ProductsIds,
                user => user.MapFrom(x => x.Products.Select(product => product.Id)))
                .ForMember(userModel => userModel.ReceiptsIds,
                user => user.MapFrom(x => x.Receipts.Select(receipt => receipt.Id)))
                .ForMember(userModel => userModel.OrdersIds,
                user => user.MapFrom(x => x.Orders.Select(order => order.Id)))
                .ReverseMap();
        }
    }
}

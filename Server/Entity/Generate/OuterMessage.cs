using LightProto;
using System;
using MemoryPack;
using System.Collections.Generic;
using Fantasy;
using Fantasy.Pool;
using Fantasy.Network.Interface;
using Fantasy.Serialize;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8618
// ReSharper disable InconsistentNaming
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable RedundantTypeArgumentsOfMethod
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PreferConcreteValueOverDefault
// ReSharper disable RedundantNameQualifier
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CheckNamespace
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable RedundantUsingDirective
// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
namespace Fantasy
{
    [Serializable]
    [ProtoContract]
    public partial class C2G_JoinRequest : AMessage, IRequest
    {
        public static C2G_JoinRequest Create(bool autoReturn = true)
        {
            var c2G_JoinRequest = MessageObjectPool<C2G_JoinRequest>.Rent();
            c2G_JoinRequest.AutoReturn = autoReturn;
            
            if (!autoReturn)
            {
                c2G_JoinRequest.SetIsPool(false);
            }
            
            return c2G_JoinRequest;
        }
        
        public void Return()
        {
            if (!AutoReturn)
            {
                SetIsPool(true);
                AutoReturn = true;
            }
            else if (!IsPool())
            {
                return;
            }
            Dispose();
        }

        public void Dispose()
        {
            if (!IsPool()) return; 
            MessageObjectPool<C2G_JoinRequest>.Return(this);
        }
        public uint OpCode() { return OuterOpcode.C2G_JoinRequest; } 
        [ProtoIgnore]
        public G2C_JoinResponse ResponseType { get; set; }
    }
    [Serializable]
    [ProtoContract]
    public partial class G2C_JoinResponse : AMessage, IResponse
    {
        public static G2C_JoinResponse Create(bool autoReturn = true)
        {
            var g2C_JoinResponse = MessageObjectPool<G2C_JoinResponse>.Rent();
            g2C_JoinResponse.AutoReturn = autoReturn;
            
            if (!autoReturn)
            {
                g2C_JoinResponse.SetIsPool(false);
            }
            
            return g2C_JoinResponse;
        }
        
        public void Return()
        {
            if (!AutoReturn)
            {
                SetIsPool(true);
                AutoReturn = true;
            }
            else if (!IsPool())
            {
                return;
            }
            Dispose();
        }

        public void Dispose()
        {
            if (!IsPool()) return; 
            ErrorCode = 0;
            id = default;
            MessageObjectPool<G2C_JoinResponse>.Return(this);
        }
        public uint OpCode() { return OuterOpcode.G2C_JoinResponse; } 
        [ProtoMember(1)]
        public uint ErrorCode { get; set; }
        [ProtoMember(2)]
        public long id { get; set; }
    }
    [Serializable]
    [ProtoContract]
    public partial class OneFrameCommand : AMessage, IMessage
    {
        public static OneFrameCommand Create(bool autoReturn = true)
        {
            var oneFrameCommand = MessageObjectPool<OneFrameCommand>.Rent();
            oneFrameCommand.AutoReturn = autoReturn;
            
            if (!autoReturn)
            {
                oneFrameCommand.SetIsPool(false);
            }
            
            return oneFrameCommand;
        }
        
        public void Return()
        {
            if (!AutoReturn)
            {
                SetIsPool(true);
                AutoReturn = true;
            }
            else if (!IsPool())
            {
                return;
            }
            Dispose();
        }

        public void Dispose()
        {
            if (!IsPool()) return; 
            frameId = default;
            foreach (var __t in commands) __t.Dispose();
            commands.Clear();
            MessageObjectPool<OneFrameCommand>.Return(this);
        }
        public uint OpCode() { return OuterOpcode.OneFrameCommand; } 
        [ProtoMember(1)]
        public int frameId { get; set; }
        [ProtoMember(2)]
        public List<Command> commands { get; set; } = new List<Command>();
    }
    [Serializable]
    [ProtoContract]
    public partial class Command : AMessage, IMessage
    {
        public static Command Create(bool autoReturn = true)
        {
            var command = MessageObjectPool<Command>.Rent();
            command.AutoReturn = autoReturn;
            
            if (!autoReturn)
            {
                command.SetIsPool(false);
            }
            
            return command;
        }
        
        public void Return()
        {
            if (!AutoReturn)
            {
                SetIsPool(true);
                AutoReturn = true;
            }
            else if (!IsPool())
            {
                return;
            }
            Dispose();
        }

        public void Dispose()
        {
            if (!IsPool()) return; 
            clientId = default;
            frameId = default;
            commandType = default;
            x = default;
            y = default;
            z = default;
            MessageObjectPool<Command>.Return(this);
        }
        public uint OpCode() { return OuterOpcode.Command; } 
        [ProtoMember(1)]
        public long clientId { get; set; }
        [ProtoMember(2)]
        public int frameId { get; set; }
        [ProtoMember(3)]
        public int commandType { get; set; }
        [ProtoMember(4)]
        public int x { get; set; }
        [ProtoMember(5)]
        public int y { get; set; }
        [ProtoMember(6)]
        public int z { get; set; }
    }
    [Serializable]
    [ProtoContract]
    public partial class G2C_StartFrameSync : AMessage, IMessage
    {
        public static G2C_StartFrameSync Create(bool autoReturn = true)
        {
            var g2C_StartFrameSync = MessageObjectPool<G2C_StartFrameSync>.Rent();
            g2C_StartFrameSync.AutoReturn = autoReturn;
            
            if (!autoReturn)
            {
                g2C_StartFrameSync.SetIsPool(false);
            }
            
            return g2C_StartFrameSync;
        }
        
        public void Return()
        {
            if (!AutoReturn)
            {
                SetIsPool(true);
                AutoReturn = true;
            }
            else if (!IsPool())
            {
                return;
            }
            Dispose();
        }

        public void Dispose()
        {
            if (!IsPool()) return; 
            clientIdList.Clear();
            foreach (var __t in positionList) __t.Dispose();
            positionList.Clear();
            MessageObjectPool<G2C_StartFrameSync>.Return(this);
        }
        public uint OpCode() { return OuterOpcode.G2C_StartFrameSync; } 
        [ProtoMember(1)]
        public List<long> clientIdList { get; set; } = new List<long>();
        [ProtoMember(2)]
        public List<Position> positionList { get; set; } = new List<Position>();
    }
    [Serializable]
    [ProtoContract]
    public partial class Position : AMessage, IDisposable
    {
        public static Position Create(bool autoReturn = true)
        {
            var position = MessageObjectPool<Position>.Rent();
            position.AutoReturn = autoReturn;
            
            if (!autoReturn)
            {
                position.SetIsPool(false);
            }
            
            return position;
        }
        
        public void Return()
        {
            if (!AutoReturn)
            {
                SetIsPool(true);
                AutoReturn = true;
            }
            else if (!IsPool())
            {
                return;
            }
            Dispose();
        }

        public void Dispose()
        {
            if (!IsPool()) return; 
            x = default;
            y = default;
            MessageObjectPool<Position>.Return(this);
        }
        [ProtoMember(1)]
        public int x { get; set; }
        [ProtoMember(2)]
        public int y { get; set; }
    }
    [Serializable]
    [ProtoContract]
    public partial class C2G_EndSync : AMessage, IMessage
    {
        public static C2G_EndSync Create(bool autoReturn = true)
        {
            var c2G_EndSync = MessageObjectPool<C2G_EndSync>.Rent();
            c2G_EndSync.AutoReturn = autoReturn;
            
            if (!autoReturn)
            {
                c2G_EndSync.SetIsPool(false);
            }
            
            return c2G_EndSync;
        }
        
        public void Return()
        {
            if (!AutoReturn)
            {
                SetIsPool(true);
                AutoReturn = true;
            }
            else if (!IsPool())
            {
                return;
            }
            Dispose();
        }

        public void Dispose()
        {
            if (!IsPool()) return; 
            MessageObjectPool<C2G_EndSync>.Return(this);
        }
        public uint OpCode() { return OuterOpcode.C2G_EndSync; } 
    }
}